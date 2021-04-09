#include "addon.h"

#pragma unmanaged

#define DECLARE_NAPI_METHOD(name, func) { name, 0, func, 0, 0, 0, napi_default, 0 }

napi_value GetDevices(napi_env env, napi_callback_info info) {
    napi_status status;
    napi_value str;

    string devices = manangerDirectInput::_getDevices();

    napi_create_string_utf8(env, devices.c_str(), NAPI_AUTO_LENGTH, &str);
    assert(status == napi_ok);

    return str;
}

napi_value GetPressedKeys(napi_env env, napi_callback_info info) {
    napi_status status;
    napi_value str;

    string pressedKeys = manangerDirectInput::_getPressedKeys();

    napi_create_string_utf8(env, pressedKeys.c_str(), NAPI_AUTO_LENGTH, &str);
    assert(status == napi_ok);

    return str;
}

napi_value SearchConnectedDevices(napi_env env, napi_callback_info info) {
     manangerDirectInput::_searchConnectedDevices();

    return nullptr;
}

/*
* @function InitNode
*
* Function that creates the addon
*
* @return   {napi_value}
*/
napi_value InitNode(napi_env env, napi_value exports)
{
    napi_status status;

    // Managed assemblies load
    // . . . . . . . . . . . . . . . . . 

    Utils::assemblerLoads();

    // Create the addon methods
    // . . . . . . . . . . . . . . . . . 

    // Obtenemos los devices

    napi_property_descriptor getDevices = DECLARE_NAPI_METHOD("GetDevices", GetDevices);
    status = napi_define_properties(env, exports, 1, &getDevices);
    assert(status == napi_ok);

    // Obtenemos las teclas pulsadas

    napi_property_descriptor getPressedKeys = DECLARE_NAPI_METHOD("GetPressedKeys", GetPressedKeys);
    status = napi_define_properties(env, exports, 1, &getPressedKeys);
    assert(status == napi_ok);

    // Buscamos dispositicos

    napi_property_descriptor searchConnectedDevices = DECLARE_NAPI_METHOD("SearchConnectedDevices", SearchConnectedDevices);
    status = napi_define_properties(env, exports, 1, &searchConnectedDevices);
    assert(status == napi_ok);

    return exports;
}

NAPI_MODULE(NODE_GYP_MODULE_NAME, InitNode)