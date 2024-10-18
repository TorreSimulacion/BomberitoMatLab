function [int,W] = focos(A, focos)

%% generacion aleatoria de fuegos
% -----------------------------------
% Constantes termodinamicas
% -----------------------------------
Ta = 298;             % temperatura ambiente
Tig = 573;            % temperatura de ignicion
ST = 1200;            % salto de temperatura al encenderse


[n,m] = size(A);
s =1 ;


W=[ ];

for i = 1 : focos  % localizacion de puntos al azar
   W = [round(rand(s,'single')*n),round(rand(s,'single')*m);W];
end



int = ones(n,m)*Ta;
[l1,l2] = size(W);

for i = 1:l1
    for j = 1:l2
        if W(i,j) == 0
           W(i,j) = 1;
        end
    end
end

for i = 1:focos
        int(W(i,1),W(i,2))=ST+Tig;
end

