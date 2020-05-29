﻿using Microsoft.Win32.SafeHandles;
using SFML.Graphics;
using System;
using System.Runtime.InteropServices;
using Xenon.Common.Object;

namespace Xenon.Common.State {
	public class GameState : IDisposable {
		public ObjectManager objectManager = new ObjectManager();

		SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
		bool disposed = false, pausedUpdate = false, pausedRender = false, isInitialized = false;

		public virtual void Load() {
			isInitialized = true;
		}

		public virtual void Update(double deltaTime) {
			if (!pausedUpdate && isInitialized) objectManager.Update(deltaTime);
			else return;
		}

		public virtual void Render(RenderWindow window) {
			if (!pausedUpdate && isInitialized) objectManager.Render(window);
			else return;
		}

		public virtual void TogglePause(int objectEvent) {
			switch (objectEvent) {
				case 1: if (!pausedUpdate) pausedUpdate = true; else pausedUpdate = false; break;
				case 2: if (!pausedRender) pausedRender = true; else pausedRender = false; break;
				default: throw new Exception("Event not recognized");
			}
		}

		public virtual void ForcePause() {
			pausedUpdate = true;
			pausedRender = true;
		}

		public virtual void ForceUnpause() {
			pausedUpdate = false;
			pausedRender = false;
		}

		public virtual void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			objectManager.Unload();
			isInitialized = false;
			if (disposed) return;
			if (disposing) { handle.Dispose(); }

			disposed = true;
		}
	}
}
