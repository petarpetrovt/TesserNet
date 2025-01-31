﻿using System;
using System.Runtime.InteropServices;

namespace TesserNet.Internal
{
    /// <summary>
    /// Provides an interface for the Tesseract API.
    /// </summary>
    internal abstract class TesseractApi
    {
        private static bool unpacked;

        /// <summary>
        /// Creates an instance of the Tesseract API for the current operating system.
        /// </summary>
        /// <returns>A Tesseract API.</returns>
        public static TesseractApi Create()
        {
            if (!unpacked)
            {
                Loader.Load();
                unpacked = true;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsTesseractApi();
            }

            return new UnixTesseractApi();
        }

        /// <summary>
        /// Creates an instance of API base.
        /// </summary>
        /// <returns>A handle for the base.</returns>
        public abstract IntPtr TessBaseAPICreate();

        /// <summary>
        /// Deletes an API base.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        public abstract void TessBaseAPIDelete(IntPtr handle);

        /// <summary>
        /// Sets the settings for the given API base. Can be executed multiple times to change settings in between runs.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        /// <param name="dataPath">The data path.</param>
        /// <param name="language">The language following ISO 639-2 specification.</param>
        /// <param name="oem">The OCR engine mode.</param>
        /// <param name="configs">The configs.</param>
        /// <param name="configSize">Size of the configuration.</param>
        /// <returns>A success code: zero if succesful, non-zero if a problem has occured.</returns>
        public abstract int TessBaseAPIInit1(IntPtr handle, string dataPath, string language, int oem, IntPtr configs, int configSize);

        /// <summary>
        /// Sets the image to be processed next.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        /// <param name="data">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bytesPerPixel">The bytes per pixel.</param>
        /// <param name="bytesPerLine">The bytes per line.</param>
        public abstract void TessBaseAPISetImage(IntPtr handle, byte[] data, int width, int height, int bytesPerPixel, int bytesPerLine);

        /// <summary>
        /// Performs the OCR.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        /// <returns>The found text on the image as a UTF8 string.</returns>
        public abstract string TessBaseAPIGetUTF8Text(IntPtr handle);

        /// <summary>
        /// Sets the source resolution.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        /// <param name="ppi">The pixels per inch.</param>
        public abstract void TessBaseAPISetSourceResolution(IntPtr handle, int ppi);

        /// <summary>
        /// Takes a rectangle of the image for performing OCR.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public abstract void TessBaseAPISetRectangle(IntPtr handle, int x, int y, int width, int height);

        /// <summary>
        /// Frees all image data and result data.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        public abstract void TessBaseAPIClear(IntPtr handle);

        /// <summary>
        /// Sets the segmentation mode.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        /// <param name="mode">The mode.</param>
        public abstract void TessBaseAPISetPageSegMode(IntPtr handle, int mode);

        /// <summary>
        /// Sets the segmentation mode.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        /// <param name="key">The name of the variable.</param>
        /// <param name="value">The value.</param>
        /// <returns>Whether the operation was succesful or not.</returns>
        public abstract bool TessBaseAPISetVariable(IntPtr handle, string key, string value);

        /// <summary>
        /// Sets the used config file.
        /// </summary>
        /// <param name="handle">The API base handle.</param>
        /// <param name="file">The name or path to the file of the config file.</param>
        public abstract void TessBaseAPIReadConfigFile(IntPtr handle, string file);
    }
}
