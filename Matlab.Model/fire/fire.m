function [U,S] = fire(U,S,W,Tmax,Sfin,alpha,k,Tig,Cs,ST,d,B,Arr)
%% Parametros del modelo

% -----------------------------------
% Parametros numericos
% -----------------------------------
[n,m] = size(S);    % tama?o grilla
% h = 1;    % paso espacial
% k = 1/10;      % paso temporal
%T = 1200;   % Tiempo final

% -----------------------------------
% Constantes termodinamicas
% -----------------------------------
% k_0 = .28;              % Conductividad termica J/m*K
% Arr = 1.8793 * 1e2;     % constante pre exponencial de arrhenius
% B = 5.5849 * 1e2;       % Energia de activacion (incluye R)
% Cs = .5;                % velocidad de quemado del combustible
% Tamb = 293;             % temperatura ambiente
%Tig = 573;              % temperatura de ignicion
% ST = 1200;              % salto de temperatura al encenderse
% rho = 1300;             % densidad de la madera kg/m3
% Cp = 1700;              % Calor espec?fico madera J/kg*K


F = @(U,S) Arr*(S.*exp(-B./(U-Tig*(U > Tig))));
G = @(U,S) (S./(1 + (k*Cs*exp(-B./(U-Tig)))));


%% Paso del tiempo en la superficie
% --------------------------------------

    
    % Actualizacion U_star -- por combustion
    U = U + F(U,S).*(U>Tig);
    
    % Actualizar U_star -- por conduccion
    Lu = del2(U,d);     
    
    U(2:n-1,2:m-1) = U(2:n-1,2:m-1) + (alpha) * Lu(2:n-1,2:m-1);
    
    focos = size(W(:,1));
    for i = 1:focos
        if U(W(i,1),W(i,2)) < ST+Tig
            if S(W(i,1),W(i,2)) > Sfin
                U(W(i,1),W(i,2)) = ST+Tig;
            end
        end
    end

    % Actualizar U
    U = U.*(U<Tmax) + Tmax.*(U>Tmax);
    
    % Actualizar S -- consumo de madera
    S = S.*(U<=Tig) + G(U,S).*(U>Tig);
    
    % Cortar la combusti?n 
    for i =1:n
        for j = 1:m
            if S(i,j)<Sfin
             S(i,j) = 0;
            end
        end
    end   
end
