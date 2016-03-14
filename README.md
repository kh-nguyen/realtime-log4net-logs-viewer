# realtime-log4net-logs-viewer
Demontration of receiving log4net logs sent from other computers using UDP protocol and JSON format and displaying them in real-time using signalR

For each computer that you would like to send the log4net's logs to the viewer, add the following appender to the log4net.config.

```xml
  <appender name="UdpAppender" type="log4net.Appender.UdpAppender">
    <!-- specify the hostname and port of the log viewer -->
    <param name="RemoteAddress" value="nmx135" />
    <param name="RemotePort" value="514" />
    <layout type="log4net.Layout.JsonLayout" />
  </appender>
```

Also, you need to install "log4net.Layout.JsonLayout" library to the software of the computer.
