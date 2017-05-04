% %This is a script that will plot Arduino analogRead values in real time
% %Modified from http://billwaa.wordpress.com/2013/07/10/matlab-real-time-serial-data-logger/
% %The code from that site takes data from Serial
% clear
% clc
% %User Defined Properties
% a = arduino('/dev/cu.usbmodem621', 'Uno');             % define the Arduino Communication port
% plotTitle = 'Arduino Data Log';  % plot title
% xLabel = 'Elapsed Time (s)';     % x-axis label
% yLabel = 'velocity (C)';      % y-axis label
% yMax  = 1000                           %y Maximum Value
% yMin  = 0                       %y minimum Value
% plotGrid = 'on';                 % 'off' to turn off grid
% min = 0;                         % set y-min
% max = 1000;                        % set y-max
% delay = .01;                     % make sure sample faster than resolution
% %Define Function Variables
% time = 0;
% data = 0;
% count = 0;
% %Set up Plot
% plotGraph = plot(time,data,'-r' )  % every AnalogRead needs to be on its own Plotgraph
% hold on                            %hold on makes sure all of the channels are plotted
% plotGraph1 = plot(time,data1,'-b')
% plotGraph2 = plot(time, data2,'-g' )
% title(plotTitle,'FontSize',15);
% xlabel(xLabel,'FontSize',15);
% ylabel(yLabel,'FontSize',15);
% legend(legend1,legend2,legend3)
% axis([yMin yMax min max]);
% grid(plotGrid);
% tic
% while ishandle(plotGraph) %Loop when Plot is Active will run until plot is closed
%     dat = a.analogRead(0)* 0.48875855327; %Data from the arduino
%     count = count + 1;
%     time(count) = toc;
%     data(count) = dat(1);
%     %This is the magic code
%     %Using plot will slow down the sampling time.. At times to over 20
%     %seconds per sample!
%     set(plotGraph,'XData',time,'YData',data);
%     axis([0 time(count) min max]);
%     %Update the graph
%     pause(delay);
% end
% delete(a);
% disp('Plot Closed and arduino object has been deleted');


clear
clc
%User Defined Properties
a = serial('/dev/cu.usbmodem411');             % define the Arduino Communication port

fopen(a);
delay = 0.001;                     % make sure sample faster than resolution

plotTitle = 'Arduino Data Log';  % plot title
xLabel = 'Elapsed Time (s)';  
yLabel = 'velocity (C)';      

y_max  = 1000;
y_min  = 0;
plotGrid = 'on';              
x_min = 0;                      
x_max = 1000;                   
%Define Function Variables
time = zeros(1000, 1);
data = zeros(1000, 1);
count = 0;
%Set up Plot
plotGraph = plot(time,data,'-r' ); 
axis([x_min x_max y_min y_max]);
grid(plotGrid);
tic

while ishandle(plotGraph)
    readData=fgets(a);

    count = count + 1;
    time(count) = toc;
    data(count) = str2double(readData);

    set(plotGraph,'XData',time,'YData',data);
    axis([0 time(count) y_min y_max]);

    pause(delay);
end

fclose(a);
disp('Plot Closed and arduino object has been deleted');