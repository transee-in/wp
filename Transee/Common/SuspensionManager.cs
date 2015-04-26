using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Transee.Common {
	internal sealed class SuspensionManager {
		public static Dictionary<string, object> SessionState { get; private set; } = new Dictionary<string, object>();
		public static List<Type> KnownTypes { get; } = new List<Type>();

		private const string SessionStateFilename = "_sessionState.xml";

		public static async Task SaveAsync() {
			try {
				foreach (var weakFrameReference in RegisteredFrames) {
					Frame frame;
					if (weakFrameReference.TryGetTarget(out frame)) {
						SaveFrameNavigationState(frame);
					}
				}

				var sessionData = new MemoryStream();
				var serializer = new DataContractSerializer(typeof (Dictionary<string, object>), KnownTypes);
				serializer.WriteObject(sessionData, SessionState);

				var file =
					await
						ApplicationData.Current.LocalFolder.CreateFileAsync(SessionStateFilename, CreationCollisionOption.ReplaceExisting);
				using (var fileStream = await file.OpenStreamForWriteAsync()) {
					sessionData.Seek(0, SeekOrigin.Begin);
					await sessionData.CopyToAsync(fileStream);
				}
			} catch (Exception e) {
				throw new SuspensionManagerException(e);
			}
		}

		public static async Task RestoreAsync(string sessionBaseKey = null) {
			SessionState = new Dictionary<string, object>();

			try {
				var file = await ApplicationData.Current.LocalFolder.GetFileAsync(SessionStateFilename);
				using (IInputStream inStream = await file.OpenSequentialReadAsync()) {
					var serializer = new DataContractSerializer(typeof (Dictionary<string, object>), KnownTypes);
					SessionState = (Dictionary<string, object>) serializer.ReadObject(inStream.AsStreamForRead());
				}

				foreach (var weakFrameReference in RegisteredFrames) {
					Frame frame;
					if (weakFrameReference.TryGetTarget(out frame) &&
					    (string) frame.GetValue(FrameSessionBaseKeyProperty) == sessionBaseKey) {
						frame.ClearValue(FrameSessionStateProperty);
						RestoreFrameNavigationState(frame);
					}
				}
			} catch (Exception e) {
				throw new SuspensionManagerException(e);
			}
		}

		private static readonly DependencyProperty FrameSessionStateKeyProperty =
			DependencyProperty.RegisterAttached("_FrameSessionStateKey", typeof (string), typeof (SuspensionManager), null);

		private static readonly DependencyProperty FrameSessionBaseKeyProperty =
			DependencyProperty.RegisterAttached("_FrameSessionBaseKeyParams", typeof (string), typeof (SuspensionManager), null);

		private static readonly DependencyProperty FrameSessionStateProperty =
			DependencyProperty.RegisterAttached("_FrameSessionState", typeof (Dictionary<string, object>),
				typeof (SuspensionManager), null);

		private static readonly List<WeakReference<Frame>> RegisteredFrames = new List<WeakReference<Frame>>();

		public static void RegisterFrame(Frame frame, string sessionStateKey, string sessionBaseKey = null) {
			if (frame.GetValue(FrameSessionStateKeyProperty) != null) {
				throw new InvalidOperationException("Frames can only be registered to one session state key");
			}

			if (frame.GetValue(FrameSessionStateProperty) != null) {
				throw new InvalidOperationException(
					"Frames must be either be registered before accessing frame session state, or not registered at all");
			}

			if (!string.IsNullOrEmpty(sessionBaseKey)) {
				frame.SetValue(FrameSessionBaseKeyProperty, sessionBaseKey);
				sessionStateKey = sessionBaseKey + "_" + sessionStateKey;
			}

			frame.SetValue(FrameSessionStateKeyProperty, sessionStateKey);
			RegisteredFrames.Add(new WeakReference<Frame>(frame));

			RestoreFrameNavigationState(frame);
		}

		public static void UnregisterFrame(Frame frame) {
			SessionState.Remove((string) frame.GetValue(FrameSessionStateKeyProperty));
			RegisteredFrames.RemoveAll(weakFrameReference => {
				Frame testFrame;
				return !weakFrameReference.TryGetTarget(out testFrame) || testFrame == frame;
			});
		}

		public static Dictionary<string, object> SessionStateForFrame(Frame frame) {
			var frameState = (Dictionary<string, object>) frame.GetValue(FrameSessionStateProperty);

			if (frameState == null) {
				var frameSessionKey = (string) frame.GetValue(FrameSessionStateKeyProperty);
				if (frameSessionKey != null) {
					if (!SessionState.ContainsKey(frameSessionKey)) {
						SessionState[frameSessionKey] = new Dictionary<string, object>();
					}
					frameState = (Dictionary<string, object>) SessionState[frameSessionKey];
				} else {
					frameState = new Dictionary<string, object>();
				}
				frame.SetValue(FrameSessionStateProperty, frameState);
			}
			return frameState;
		}

		private static void RestoreFrameNavigationState(Frame frame) {
			var frameState = SessionStateForFrame(frame);
			if (frameState.ContainsKey("Navigation")) {
				frame.SetNavigationState((string) frameState["Navigation"]);
			}
		}

		private static void SaveFrameNavigationState(Frame frame) {
			var frameState = SessionStateForFrame(frame);
			frameState["Navigation"] = frame.GetNavigationState();
		}
	}

	public class SuspensionManagerException : Exception {
		public SuspensionManagerException() {
		}

		public SuspensionManagerException(Exception e)
			: base("SuspensionManager failed", e) {
		}
	}
}