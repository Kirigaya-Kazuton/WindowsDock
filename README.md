# WindowsDock

Mac OS X like application launcher.

![Preview](http://www.neptuo.com/Content/Images/Projects/windows-dock-01.jpg)

---

## 🎯 Recursos e Funcionalidades

### ⚡ Atalhos (Shortcuts)

- **Arrastar e Soltar** - Simplesmente arraste qualquer arquivo, pasta ou executável para a dock para criar um atalho
- **Atalhos Personalizados** - Configure manualmente atalhos com caminhos, argumentos e diretório de trabalho
- **Hotkeys Globais** - Atribua hotkeys (letras + números) para executar atalhos rapidamente com `Win + Tecla`
- **Múltiplos Atalhos** - Vários atalhos podem compartilhar a mesma hotkey para executar múltiplos programas
- **Ícones Customizáveis** - Cada atalho exibe o ícone do aplicativo associado
- **Argumentos Dinâmicos** - Passe argumentos ao executar atalhos (ex: abrir arquivos específicos)

### 📝 Textnotes & Alarmes

- **Notas de Texto Simples** - Crie, edite e organize notas rápidas
- **Sistema de Alarmes** - Configure alarmes para lembrar de tarefas importantes
- **Som de Alarme** - Notificações sonoras customizáveis
- **Filtro de Notas** - Busque rapidamente por suas notas usando filtros
- **Persistência** - Todas as notas são salvas automaticamente

### 🔧 Scripts

- **Executar Scripts** - Crie atalhos para seus scripts favoritos (.bat, .ps1, .vbs, etc)
- **Diretório de Trabalho** - Configure o diretório onde o script será executado
- **Gerenciamento Avançado** - Interface completa para gerenciar todos os scripts

### 📁 Navegador de Pastas (Browser)

- **Navegação Rápida** - Digite caminhos (ex: `C:\` ou `D:\Users\Documents\`)
- **Navegação com Teclado** - Use setas para navegar e Enter para abrir pastas
- **Comandos Personalizados** - Defina comandos customizados para pastas (ex: abrir no Explorer, no terminal, etc)
- **Comando Padrão** - Um comando pode ser definido como padrão e ativado com Enter

### 🖥️ Desktop Browser

- **Navegação de Desktop** - Acesse rapidamente todos os itens na sua área de trabalho
- **Filtro Desktop** - Procure por arquivos e pastas do desktop
- **Execução Direta** - Abra arquivos do desktop através da dock

### ⌨️ Atalhos de Teclado

| Tecla | Função |
|-------|--------|
| **Win + W** | Ativa/Desativa a dock |
| **Win + T** | TextNotes |
| **Win + S** | Scripts |
| **Win + B** | Navegador de Pastas |
| **Win + D** | Desktop Browser |
| **Win + Z** | Configurações |
| **Win + X** | Fechar dock |
| **[Customizável]** | Atalho para cada shortcut |

### 🎨 Personalização & Configuração

#### Aparência
- **Cor de Fundo** - Customize a cor do painel da dock (suporta alpha channel para transparência)
- **Opacidade** - Ajuste a transparência da janela
- **Raio de Canto** - Customize o arredondamento dos cantos
- **Espessura de Borda** - Defina a espessura da borda da dock
- **Cor de Borda** - Escolha a cor da borda
- **Cores de Botões** - Customize cores dos botões de config e fechar

#### Posicionamento & Encaixe
- **Posição** - Coloque a dock no topo, embaixo, esquerda ou direita da tela
- **Alinhamento** - Alinhe a dock ao centro, início ou fim do espaço disponível
- **Offset de Alinhamento** - Ajuste fino da posição com pixels customizáveis
- **Encaixe (Docking)** - Fixe a dock em uma borda para mantê-la sempre visível
- **Suporte Multi-Monitor** - Posicione em diferentes bordas da tela primária

#### Comportamento
- **Auto-Hide** - A dock se esconde automaticamente quando o mouse sai dela
- **Duração de Show/Hide** - Configure velocidade de animação
- **Delay de Hide** - Tempo de espera antes de esconder
- **Iniciar Oculto** - A dock começa oculta quando o aplicativo inicia
- **Altura da Taskbar** - Configure offset baseado na altura da taskbar do Windows

#### Interface
- **Bolhas de Atalho** - Mostre/oculte labels com nomes dos atalhos
- **Tamanho da Font** - Customize tamanho da fonte das bolhas
- **Tamanho do Ícone** - Ajuste o tamanho dos ícones dos atalhos
- **Botão de Config** - Mostre/oculte botão de configurações
- **Botão de Fechar** - Mostre/oculte botão de fechar

#### Localização
- **Idioma** - Suporte para múltiplos idiomas (Português, Inglês, Tcheco, etc)
- **Fácil Extensão** - Adicione novos idiomas editando arquivos de recursos

### 💾 Gerenciamento de Dados

- **Salvar Configuração** - Salve suas configurações manualmente
- **Carregar Configuração** - Recarregue configurações anteriormente salvas
- **Substituir Configuração** - Importe configurações de outro arquivo
- **Copiar Arquivo** - Crie cópias de backup de suas configurações
- **Restaurar Padrões** - Restaure configurações, atalhos, notas e scripts para padrões
- **Armazenamento Isolado** - Dados armazenados de forma segura em Isolated Storage

### 🎛️ Configurações Avançadas

- **Hotkey Customizável** - Mude a tecla de ativação principal (padrão: Win+W)
- **Diretório de Trabalho** - Configure diretório padrão para atalhos e scripts
- **Offset Oculto** - Número de pixels sempre visíveis quando a dock está oculta
- **Menu de Contexto** - Clique direito para acessar menu alternativo de configurações

---

## 📋 Atalhos do Changelog (Por Versão)

### Build 1.2.22
- Correção menor (remoção da feia borda em volta do atalho no painel principal)

### Build 1.2.21 
- Hotkeys globais para atalhos (apenas em combinação com tecla Windows)

### Build 1.1.20
- Configuração para bolhas pequenas em cima de atalhos no painel principal
- Configuração básica para tamanho do ícone dos atalhos

### Build 1.1.19
- Altura da taskbar + possibilidade de usar como offset a partir do fundo

### Build 1.1.18
- Dock encaixável, você pode fixar o painel principal em uma borda da tela e deixar sempre visível
- Correções menores

### Build 1.0.17
- Cores para botões de aplicação (Config + Close)
- Possibilidade de ocultá-los do painel principal
- Menu de contexto no painel principal como substituto destes botões

### Build 1.0.16
- Atalho de teclado para mostrar o desktop no Explorer

### Build 1.0.15
- Suporte adicionado para fixar em borda direita e inferior da tela primária

### Build 1.0.14
- Possibilidade de configurar tecla de ativação (Win + tecla selecionada)
- Reinicialização do aplicativo quase não necessária após mudança de localização

---

## 🎮 Telas

### Build 1.0.12
![Build 1.0.12](http://i51.tinypic.com/2mot25u.png)

### Build 1.0.1
![Build 1.0.1](http://i55.tinypic.com/2mo5bh3.png)

### Build 1.0.0
![Build 1.0.0](http://i55.tinypic.com/9ap7pv.png)

---

## 🚀 Primeiros Passos

### Instalação

1. Baixe a versão mais recente do [repositório](https://github.com/Kirigaya-Kazuton/WindowsDock)
2. Execute o instalador (WindowsDock.Setup)
3. Inicie a aplicação

### Uso Básico

1. **Adicionar um Atalho**: Arraste um arquivo/pasta/executável para a dock
2. **Abrir Configurações**: Pressione `Win+Z` ou clique no botão de engrenagem
3. **Ativar/Desativar**: Pressione `Win+W`
4. **Explorar Extensões**: Use os botões na dock ou seus hotkeys (`Win+T`, `Win+S`, `Win+B`, `Win+D`)

### Dica: Workflow Rápido

1. Configure atalhos para seus aplicativos mais usados
2. Atribua hotkeys rápidos (ex: Win+P para Photoshop, Win+C para Chrome)
3. Ative Auto-Hide e Iniciar Oculto para uma dock minimalista
4. Use o Navegador de Pastas para acesso rápido a diretórios frequentes

---

## 📝 License

Este projeto é um fork de [maraf/WindowsDock](https://github.com/maraf/WindowsDock)

Visite [neptuo.com/project/desktop/windowsdock](http://www.neptuo.com/project/desktop/windowsdock/) para mais informações
