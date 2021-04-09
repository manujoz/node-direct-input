#include "manangerDirectInput.h"

namespace manangerDirectInput
{
	string _getDevices()
	{
		System::String^ devices = CS::deviceclass->GetDevices();
		return Utils::convert_from_cs_string(devices);
	}

	string _getPressedKeys()
	{
		System::String^ pressedKeys = CS::deviceclass->GetPressedKeys();
		return Utils::convert_from_cs_string(pressedKeys);
	}

	void _searchConnectedDevices()
	{
		CS::deviceclass->SearchConnectedDevices();
	}
}