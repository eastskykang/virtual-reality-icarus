clear
clc

if ~isempty(instrfind('Tag', 'Arduino'))
    fclose(instrfind('Tag', 'Arduino'));
end

% User Defined Properties
a = serial('/dev/cu.usbmodem621');             % define the Arduino Communication port
set(a, 'Tag', 'Arduino')

fopen(a);
delay = 0.001;                     % make sure sample faster than resolution

% Plot 
plotGrid = 'on';

% Define Function Variables
time = zeros(1000, 1);
data1 = zeros(1000, 1);
data2 = zeros(1000, 1);
data3 = zeros(1000, 1);
count = 0;

% Set up plot
figure(1)
subplot(3, 1, 1)
plotGraph1 = plot(time, data1, '.r');
axis([0 1 0 10]);
title('cycle velocity')
xlabel('time (sec)')
ylabel('frequency (Hz)')
grid(plotGrid);
ax1 = gca;

subplot(3, 1, 2)
plotGraph2 = plot(time, data2, '.g');
axis([0 1 -1100 1100]);
title('handle angle')
xlabel('time (sec)')
ylabel('angle')
grid(plotGrid);
ax2 = gca;

subplot(3, 1, 3)
plotGraph3 = plot(time, data3, '.b');
axis([0 1 0 1200]);
title('sound input')
xlabel('time (sec)')
ylabel('sound')
grid(plotGrid);
ax3 = gca;

tic

while ishandle(plotGraph1)
    readData1 = fgets(a);   % line 1
    readData2 = fgets(a);   % line 2
    readData3 = fgets(a);   % line 3
    
    % variable init
    angle = 0;
    velocity = 0;
    sound = 0;
    
    % parse packet 
    try
        [angle, velocity, sound] = VariableFromPacket(...
            ParsePacket(readData1), ...
            ParsePacket(readData2), ...
            ParsePacket(readData3));
    catch ME
        disp(ME)
    end
        
    count = count + 1;
    time(count) = toc;
    data1(count) = velocity;
    data2(count) = angle;
    data3(count) = sound;
    
    set(plotGraph1, 'XData', time, 'YData', data1);
    set(plotGraph2, 'XData', time, 'YData', data2);
    set(plotGraph3, 'XData', time, 'YData', data3);
    
    if count > 1000
        ax1.XLim = [time(count - 1000) time(count)];
        ax2.XLim = [time(count - 1000) time(count)];
        ax3.XLim = [time(count - 1000) time(count)];
    else
        ax1.XLim = [0 time(count)];
        ax2.XLim = [0 time(count)];
        ax3.XLim = [0 time(count)];
    end
    
    pause(delay);
end

fclose(a);
disp('Plot Closed and arduino object has been deleted');

%% FUNCTIONS
function [tv] = ParsePacket(str)
    
    % Packets
    anglePrefix = 'AN';
    velocityPrefix = 'VE';
    soundPrefix = 'DB';
    
    cells = strsplit(str, '::');
    
    if size(cells, 2) ~= 2
        ME = MException('parse failed: %s', str);
        throw(ME)    
    end
    
    if strcmp(cells{1}, anglePrefix)
        type = 1;
        value = str2double(cells{2});
    elseif strcmp(cells{1}, velocityPrefix)
        type = 2;
        value = str2double(cells{2});
    elseif strcmp(cells{1}, soundPrefix)
        type = 3;
        value = str2double(cells{2});
    else
        ME = MException('parse failed: %s', str);
        throw(ME)    
    end
    
    tv = {type, value};
end

function [angle, velocity, sound] = VariableFromPacket(t1v1, t2v2, t3v3)
    
    angle = 0;
    velocity = 0;
    sound = 0;
    
    t1 = t1v1{1};
    v1 = t1v1{2};
    
    t2 = t2v2{1};
    v2 = t2v2{2};
    
    t3 = t3v3{1};
    v3 = t3v3{2};
    
    if t1 == 1
        angle = v1;
    elseif t1 == 2
        velocity = v1;
    elseif t1 == 3
        sound = v1;
    else
        assert('parse failed')
    end
    
    if t2 == 1
        angle = v2;
    elseif t2 == 2
        velocity = v2;
    elseif t2 == 3
        sound = v2;
    else
        assert('parse failed')
    end
    
    if t3 == 1
        angle = v3;
    elseif t3 == 2
        velocity = v3;
    elseif t3 == 3
        sound = v3;
    else
        assert('parse failed')
    end
end