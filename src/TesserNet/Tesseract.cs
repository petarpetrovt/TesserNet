﻿using System;
using TesserNet.Internal;

namespace TesserNet
{
    /// <summary>
    /// Provides high level bindings for the Tesseract API.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class Tesseract : IDisposable
    {
        private readonly TesseractApi api;
        private readonly IntPtr handle;
        private readonly object lck = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Tesseract"/> class.
        /// </summary>
        public Tesseract()
            : this(new TesseractOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tesseract"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Tesseract(TesseractOptions options)
        {
            Options = options;
            api = TesseractApi.Create();
            handle = api.TessBaseAPICreate();
        }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public TesseractOptions Options { get; set; }

        /// <summary>
        /// Performs OCR on the given image.
        /// </summary>
        /// <param name="data">The bytes of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
        /// <returns>The found text as a UTF8 string.</returns>
        public string Read(byte[] data, int width, int height, int bytesPerPixel)
        {
            lock (lck)
            {
                Init();
                api.TessBaseAPISetImage(handle, data, width, height, bytesPerPixel, width * bytesPerPixel);
                api.TessBaseAPISetSourceResolution(handle, Options.PixelsPerInch);
                return api.TessBaseAPIGetUTF8Text(handle);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            api.TessBaseAPIDelete(handle);
        }

        private void Init()
        {
            api.TessBaseAPIInit1(handle, Options.DataPath, Options.Language, (int)Options.EngineMode, IntPtr.Zero, 0);
        }
    }
}
