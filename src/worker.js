const addon = require("bindings")("node-direct-input");
const { parentPort } = require("worker_threads");

class WorkerDirectInput {
    constructor() {
        this.cont = 0;
        this.stop = false;
        this.stopped = false;

        this.lastDevicesStr = "[]";
        this.lastKeyPressedStr = "[]";
    }

    listen() {
        if (this.cont === 0) {
            addon.SearchConnectedDevices();
            const devices = addon.GetDevices();
            this.devicesDetected(devices);
        }

        const keysPressed = addon.GetPressedKeys();
        this.keysPressed(keysPressed);

        if (this.cont === 50) {
            this.cont = 0;
        } else {
            this.cont++;
        }

        if (!this.stop) {
            setTimeout(() => {
                this.listen();
            }, 50);
        } else {
            this.cont = 0;
            this.stop = false;
        }
    }

    devicesDetected(devices) {
        if (devices !== this.lastDevicesStr) {
            this.lastDevicesStr = devices;
            parentPort.postMessage({ evName: "devices", result: devices });
        }
    }

    keysPressed(keysPressed) {
        if (keysPressed !== this.lastKeyPressedStr) {
            this.lastKeyPressedStr = keysPressed;
            parentPort.postMessage({ evName: "keysPressed", result: keysPressed });
        }
    }
}

const worker = new WorkerDirectInput();

parentPort.on("message", (response) => {
    if (response.listen != undefined) {
        worker.listen();
    }

    if (response.stop !== undefined) {
        worker.stop = true;
    }
});
