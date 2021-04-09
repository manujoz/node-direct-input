const fs = require("fs");
const path = require("path");
const addon = require("bindings")("node-direct-input");
const { Worker } = require("worker_threads");
const events = require("events").EventEmitter;

class NodeDirectInput extends events {
    constructor() {
        super();

        this.devices = [];
        this.lastKeyPressedStr = "";

        this._worker = null;
    }

    getDevicesConnected() {
        addon.SearchConnectedDevices();
        const devices = addon.GetDevices();
        return JSON.parse(devices);
    }

    listen() {
        if (!this._worker) {
            this._worker = new Worker(path.resolve(__dirname, "worker.js"));

            this._worker.postMessage({
                listen: true,
            });

            this._worker.on("message", (response) => {
                if (response.evName !== undefined) {
                    this._get_result(response.evName, response.result);
                }
            });

            this._worker.on("error", (error) => {
                console.error("[node-direct-input]: ", error);
            });

            this._worker.on("exit", () => {});
        } else {
            this._worker.postMessage({
                listen: true,
            });
        }
    }

    stop() {
        if (!this._worker) {
            return;
        }

        this._worker.postMessage({
            stop: true,
        });
    }

    _get_result(evName, result) {
        if (evName === "devices") {
            this.devices = JSON.parse(result);
            this.emit("ndi:devices", this.devices);
        }

        if (evName === "keysPressed") {
            const keyPressed = JSON.parse(result);
            if (keyPressed.length > 0) {
                this.emit("ndi:keysPressed", JSON.parse(result));
            }
        }
    }
}

module.exports.NodeDirectInput = NodeDirectInput;
