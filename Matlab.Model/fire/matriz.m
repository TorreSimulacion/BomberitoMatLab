%formacion de la grilla
function A = matriz(Lx,Ly,d)
%A = ones(n,n);
A = ones(Lx/d,Ly/d);
% A = horzcat (zeros (n-2,1),ones (n-2),zeros (n-2,1));  % matriz de simulacion
% A = vertcat (zeros(1,j),A,zeros(1,j)); % contorno
% A = A * 298;
% A (n/2,j/2)=1000;
end
