// This file is provided under The MIT License as part of Steamworks.NET.
// Copyright (c) 2013-2017 Riley Labrecque
// Please see the included LICENSE.txt for additional information.

// This file is automatically generated.
// Changes to this file will be reverted when you update Steamworks.NET

#if !DISABLESTEAMWORKS

using System;
using System.Runtime.InteropServices;

namespace Steamworks {
	public static class SteamScreenshots {
		/// <summary>
		/// <para> Writes a screenshot to the user's screenshot library given the raw image data, which must be in RGB format.</para>
		/// <para> The return value is a handle that is valid for the duration of the game process and can be used to apply tags.</para>
		/// </summary>
		public static ScreenshotHandle WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight) {
			InteropHelp.TestIfAvailableClient();
			return (ScreenshotHandle)NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
		}

		/// <summary>
		/// <para> Adds a screenshot to the user's screenshot library from disk.  If a thumbnail is provided, it must be 200 pixels wide and the same aspect ratio</para>
		/// <para> as the screenshot, otherwise a thumbnail will be generated if the user uploads the screenshot.  The screenshots must be in either JPEG or TGA format.</para>
		/// <para> The return value is a handle that is valid for the duration of the game process and can be used to apply tags.</para>
		/// <para> JPEG, TGA, and PNG formats are supported.</para>
		/// </summary>
		public static ScreenshotHandle AddScreenshotToLibrary(string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight) {
			InteropHelp.TestIfAvailableClient();
			using (var pchFilename2 = new InteropHelp.UTF8StringHandle(pchFilename))
			using (var pchThumbnailFilename2 = new InteropHelp.UTF8StringHandle(pchThumbnailFilename)) {
				return (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(pchFilename2, pchThumbnailFilename2, nWidth, nHeight);
			}
		}

		/// <summary>
		/// <para> Causes the Steam overlay to take a screenshot.  If screenshots are being hooked by the game then a ScreenshotRequested_t callback is sent back to the game instead.</para>
		/// </summary>
		public static void TriggerScreenshot() {
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_TriggerScreenshot();
		}

		/// <summary>
		/// <para> Toggles whether the overlay handles screenshots when the user presses the screenshot hotkey, or the game handles them.  If the game is hooking screenshots,</para>
		/// <para> then the ScreenshotRequested_t callback will be sent if the user presses the hotkey, and the game is expected to call WriteScreenshot or AddScreenshotToLibrary</para>
		/// <para> in response.</para>
		/// </summary>
		public static void HookScreenshots(bool bHook) {
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
		}

		/// <summary>
		/// <para> Sets metadata about a screenshot's location (for example, the name of the map)</para>
		/// </summary>
		public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation) {
			InteropHelp.TestIfAvailableClient();
			using (var pchLocation2 = new InteropHelp.UTF8StringHandle(pchLocation)) {
				return NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, pchLocation2);
			}
		}

		/// <summary>
		/// <para> Tags a user as being visible in the screenshot</para>
		/// </summary>
		public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
		}

		/// <summary>
		/// <para> Tags a published file as being visible in the screenshot</para>
		/// </summary>
		public static bool TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
		}

		/// <summary>
		/// <para> Returns true if the app has hooked the screenshot</para>
		/// </summary>
		public static bool IsScreenshotsHooked() {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamScreenshots_IsScreenshotsHooked();
		}

		/// <summary>
		/// <para> Adds a VR screenshot to the user's screenshot library from disk in the supported type.</para>
		/// <para> pchFilename should be the normal 2D image used in the library view</para>
		/// <para> pchVRFilename should contain the image that matches the correct type</para>
		/// <para> The return value is a handle that is valid for the duration of the game process and can be used to apply tags.</para>
		/// <para> JPEG, TGA, and PNG formats are supported.</para>
		/// </summary>
		public static ScreenshotHandle AddVRScreenshotToLibrary(EVRScreenshotType eType, string pchFilename, string pchVRFilename) {
			InteropHelp.TestIfAvailableClient();
			using (var pchFilename2 = new InteropHelp.UTF8StringHandle(pchFilename))
			using (var pchVRFilename2 = new InteropHelp.UTF8StringHandle(pchVRFilename)) {
				return (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddVRScreenshotToLibrary(eType, pchFilename2, pchVRFilename2);
			}
		}
	}
}

#endif // !DISABLESTEAMWORKS
