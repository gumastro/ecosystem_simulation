# Simulando um Ecossistema
[![Watch the video](https://img.youtube.com/vi/W7A6g8MFuAU/maxresdefault.jpg)](https://www.youtube.com/watch?v=W7A6g8MFuAU)
Nosso projeto consiste na simulação de um ecossistema onde duas espécies interagem como predador e presa, sendo elas raposas e coelhos, respectivamente. Tivemos vontade de desenvolver esse projeto quando assistimos ao _Coding Adventure: Simulating an Ecosystem_ (https://www.youtube.com/watch?v=r_It_X7v-1E), pois o algoritmo evolutivo ficou muito visual e queríamos ter uma implementação própria similar.

Nosso sistema gira em torno de 3 barras de status:
```
• Fome
• Sede
• Maturidade para reprodução
```

E 6 pontos de evolução:
```
• Velocidade
• Campo de visão
• Sede/Fome
• Taxa de reprodução
• Coragem
• Cor
```
## Funcionamento do sistema 
As barras de status de fome e sede sobem igualmente para toda a população, enquanto a de maturidade para reprodução depende da taxa de reprodução de cada indivíduo.

Um indivíduo só pode se reproduzir se não sente fome nem sede e tem sua barra de maturidade completa.

Ao ter completa sua barra de fome, sede ou serem abatidos por outra espécie, o indivíduo é eliminado.

Ao coelho detectar um predador, ele tenta escapar na direção oposta.

## Pontos de evolução
**Velocidade e campo de visão**, quanto maiores os valores, melhor pro indivíduo, pois facilita sua busca e competição por alimento.

**Sede/Fome** regula a que ponto a barra de status precisa estar preenchida para que o indivíduo **sinta a necessidade de buscar** água ou alimento. Importante para que o indivíduo encontre uma **janela de oportunidade** ótima para **reprodução**, uma vez que ela só é possível quando não se sente fome nem sede.

A **taxa de reprodução** busca um **período** ótimo para o indivíduo alcançar a **maturidade para reprodução**. O objetivo desse ponto é regular a quantidade de indivíduos na população dada a quantidade de alimentos disponíveis.

**Coragem** determina uma distância de **tolerância** da presa ao detectar um **predador**, uma vez que o coelho não pode ser abatido mas também não pode morrer de fome nem sede.

A **cor** apenas representa **estética** e nos mostra que os indivíduos estão **convergindo** para certa **configuração**.

## Termos técnicos
O **fitness** de cada indivíduo é determinado pela quantidade de **tempo** que ele sobrevive. Portanto, um indivíduo que sobrevive por mais tempo tem mais chances de se reproduzir, ou seja, os genes “bons” têm mais **chances de serem propagados**.

A relação entre raposas e coelhos é de **predação randômica** (baseado na proximidade física entre eles).

**Crossover** é uma média aritmética (((pai+mãe)/2) + mutação).

## Autores
* [Gustavo Tuani Mastrobuono](https://github.com/gumastro)  
* [Paulo da Silva](https://github.com/pau1o-hs)
