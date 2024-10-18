function S = combustible(A)
%Definir distribucion inicial del combustible
%S_int = ones(size(U_int));

[n,m] = size(A);
S_int = ones(size(A));
S_int = reshape(A,n*m,1);

% Randomizar distribucion combustible
Ran = rand(size(S_int));
S_int = S_int - (.7*Ran);
S_int = S_int.*(Ran > 0.7) + 0.1*(Ran < 0.7);

% Frenar distribucion del fuego
% r = n/10; % numero de columnas hasta los ceros
% S_int(n^2/5:n*(n/5+r)) = zeros(size(S_int(n*10:n*(10+r))));
S = S_int;
S = reshape(S,n,m);  

%S_plot_int = (reshape(S,n,n))';
end
