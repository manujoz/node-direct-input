# Node Direct Input

It detects events for keyboard keys and gaming device buttons (not axes) even when the focus is not on the program, and it does not block events for other open applications.

This module is designed to complement games with external applications in which you need to know when a key or button on the keyboard or any gaming device that is connected to the computer has been pressed. If you need more functionalities, you have a base made and knowing a little C# and C++ you are free to adapt it to your needs if, for example, you need to also obtain the axes of the game devices or mouse events among other things.

## Install

```
npm i node-direct-input
```

## Build

The package is compiled for the node.js version 12.16.0, it is possible to make reconstructions for other node versions, but a correct operation of the versions prior to 12 is not guaranteed. The module makes use of _Worker Threads_ which is Available only from version 11.7.0 of node.js and other plugins that are only available in the newer versions of nodejs.

To build the node package, just run the following command after installing the development dependencies:

```shell
$ npm run rebuild
```

If you want to use the package in a different execution environment such as NWJS or Electron, there are two ways to compile the module. The first is to edit the _package.json_ and modify the following lines to suit your version of NWJS or Electron:

```javascript
"scripts": {
    "test": "node test/test.js",
    "clean": "cmake-js clean",
    "build": "cmake-js build -G \"Visual Studio 15 2017 Win64\"",
    "rebuild": "cmake-js rebuild -G \"Visual Studio 15 2017 Win64\"",
    "build-nw": "cmake-js build --runtime=nw --runtime-version=<<YOUR-NW-VERSIONI>> --arch=x64 -G \"Visual Studio 15 2017 Win64\"",
    "rebuild-nw": "cmake-js rebuild --runtime=nw --runtime-version=<<YOUR-NW-VERSIONI>> --arch=x64 -G \"Visual Studio 15 2017 Win64\"",
    "build-electron": "cmake-js build --runtime=electron --runtime-version=<<YOUR-ELECTRON-VERSIONI>> --arch=x64 -G \"Visual Studio 15 2017 Win64\"",
    "rebuild-electron": "cmake-js build --runtime=electron --runtime-version=<<YOUR-ELECTRON-VERSIONI>> --arch=x64 -G \"Visual Studio 15 2017 Win64\""
}
```

Replace the text _\<\<YOUR-XX-VERSION>>_ with the NWJS or electron version in which the module will be compiled and then execute the corresponding script depending on where you are compiling the module

```shell
# For NWJS
$ npm run rebuild-nw

# For Electrón
$ npm run rebuild-electron
```

You can also do a more specific manual compilation by following the instructions in <a href="https://github.com/cmake-js/cmake-js">cmake-js</a>.

## Use

```javascript
const nodeDirectInput = require("node-direct-input");
const di = new nodeDirectInput();
```

### Listen key or games device buttons events

```javascript
// Start events listen
di.listen();

// Event dispatch when a new device is connected or disconnected
di.on("ndi:devices", (devices) => {
  console.log(devices);
});

// Event dispatch when key or button presseds change
di.on("ndi:keysPressed", (keysPressed) => {
  console.log(keysPressed);
});
```

### Stop listen

```javascript
di.stop();
```

### Request connected devices

Is not necesary be listening

```javascript
const devices = di.getDevicesConnected();
console.log(devices);
```
