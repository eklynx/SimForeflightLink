# SimForeflightLink
A barebones app to send SimConnect data to Foreflight.

## Application status

### Application Settings:
Settings are currently saved automatically on change.  The current settings are the Auto-Connect to SimConnect, Auto-Connect to ForeFlight, and the DirectIP where to send ForeFlight packet.

Note: Autostart is only applicable on application start.  Checking the box will not start the conenction by itself; you need to click on the Connect button.

### SimConnect:
Currently only works with the local memory pipe.  Will add network capabilities in the future.  If in an active state, it will keep retrying to connection if it cannot be made or if it drops.

### ForeFlight:
Currently supports specified IP.  Will add dropdown for network broadcasts as well.  IP address entry is not validated and no error handling exists yet, so be sure that there is a valid IP address before hitting 'connect'.  For most home networks, to send to the network as a broadcst, replace the last section of the IP address to 255.  For example, if your ip address is 192.168.0.45, send the packets to 192.168.0.255. 

Also, the Foreflight sender will not send unless all the data is populated.

## To Build Locally
Because I have not verified licensing of the SimConnect DLLs, They are not included in this project.  For building the application, place both `SimConnect.dll` and `Microsoft.FlightSimulator.SimConnect.dll` in the `lib` folder of the project.

