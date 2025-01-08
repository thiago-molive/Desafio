Desafio proposto concluir em 5 dias ou menos as questões (dentro da pasta src)


### Instruções

-Abra o projeto no visual studio 2022 (ou editor da sua preferencia) usando o Exercicio.sln na pasta src.
-Faça um build do projeto para baixar as dependencias
-Clique com o botão direito e selecione o projeto a ser inicializado
-Execute o projeto

### Desafio 1
Usei uma abordagem com DDD para realiar o desafio e escrevi testes de unidade para as classes que criei

### Desafio 2
Usei refit para realizar a integração com a api de mock, configurei injeção de dependencia para facilitar, ignorei os logs de inicialização no console para não atrapalhar a visualização
os models estão na pasta domain e a integração com a api na pasta integration

### Desafio 3
Fiquei com dúvida, estou aguardando a resposta de como proceder...

### Desafio 4
A resposta também está no arquivo da questão 4
```sql
SELECT ASSUNTO,
ANO,
COUNT(1) QUANTIDADE
FROM ATENDIMENTOS
GROUP BY ASSUNTO,ANO
HAVING COUNT(1) >  3
ORDER BY ANO DESC, QUANTIDADE DESC 
```

### Questão 5
