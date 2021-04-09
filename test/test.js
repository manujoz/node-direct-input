const { NodeDirectInput } = require("../src/node-direct-input");

const directInput = new NodeDirectInput();

// You can request connected devices width this method

// const devices = directInput.getDevicesConnected();
// console.log(devices);

directInput.listen();
directInput.on("ndi:devices", (devices) => {
    console.log(devices);
});
directInput.on("ndi:keysPressed", (keysPressed) => {
    console.log(keysPressed);
});

setTimeout(() => {
    console.log("STOP");
    directInput.stop();
}, 10000);
setTimeout(() => {
    console.log("START");
    directInput.listen();
}, 12000);
