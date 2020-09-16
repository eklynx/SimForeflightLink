# SimForeflightLink
A barebones app to send SimConnect data to Foreflight.

## Application status
### SimConnect:
Currently only works with the local memory pipe.  Will add network capabilities in the future.  If in an active state, it will keep retrying to connection if it cannot be made or if it drops.

### ForeFlight:
Currently supports specified IP.  Will add dropdown for network broadcasts as well.  IP address entry is not validated and no error handling exists yet, so be sure that there is a valid IP address before hitting 'connect'.  

### Application Settings
There is no saving of settings between sessions. This will be implemented soon.  Because of this, the autostart buttons are disabled. Once settings can be saved, we will enable the atuo-start options.

## To Build Locally
Because I have not verified licensing of the SimConnect DLLs, They are not included in this project.  For building the application, place both `SimConnect.dll` and `Microsoft.FlightSimulator.SimConnect.dll` in the `lib` folder of the project.

