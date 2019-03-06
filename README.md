# Projet Génie Logiciel - Intranet Casablanca


Bienvenue dans le GitHub officiel du projet Génie Logiciel de la team Cheval de Métal

Ce projet consiste à développer un intranet gérant les notes de frais ainsi que les 
demande de congés des collaborateurs de l'entreprise Pops1819.

## Description du projet

L’objectif de ce projet est de développer un site intranet pour l’entreprise Pops1819. 

Cet intranet a pour objectif de satisfaire les besoins suivants :
- permettre les demandes de congés par les collaborateurs, ainsi que leur visualisation et validation par leur chef de service respectif ainsi que le service RH
- permettre les demandes de notes de frais, la mise en ligne de justificatifs et leur validation par le chef de service concerné et le service Compta
- permettre les demandes d'avance et leur validation

L’entreprise Pops1819 est composée d’une dizaine de services possédant chacun un chef de service. Celui-ci valide les demandes de congés de ses collaborateurs et de notes de frais liées aux missions dont il est responsable : il est nécessaire qu’il possède une visualisation avancée de la situation de l’entreprise en terme de congé et de missions. L’intranet devra l’aider dans sa tâche et afficher les informations pertinentes pour la prise de décision. 

L’entreprise contient également un service Comptabilité qui traite notamment les notes de frais, d’un service Ressources Humaines traitant notamment les demandes de congés, ainsi que d’un service Direction. 

Cet intranet est destiné à tout collaborateur de l’entreprise et doit être accessible sur différentes plateformes (tablette, mobile, ordinateur). L’utilisation du site ne doit pas demander de connaissances ou de compétences particulières. Chaque collaborateur disposant d’un rôle spécial (collaborateurs des services RH et Comptabilité, Chef de service…) auront accès à une interface spécial leur permettant de traiter les demandes des autres collaborateurs.

L’utilisateur du site sera destiné à le consulter au moins une fois par semaine jusqu’à plusieurs fois par jour. De fait, le temps d’utilisation requis pour effectuer une demande des congés, un remboursement de frais ou des informations devra être le plus court possible et le processus le plus organique possible, afin que les collaborateurs puissent se consacrer pleinement à leurs missions et projets. 

Chaque collaborateur de l’entreprise n’appartient qu’à un seul service mais peuvent servir dans plusieurs missions de plusieurs services. Les collaborateurs ont interdiction de valider leurs propres demandes, c’est pourquoi il faut réfléchir à chaque cycle de demandes et surtout celles qui incluent une absence d’un collaborateur clé dans le traitement de celle-ci. 

## Développement

### Installation du projet

Il est nécessaire d'installer des extensions de Visual Studio pour lancer le projet : ce sont les outils dans la catégorie ASP.NET and web development.

### Utilisation du site web

Pour en savoir plus sur l’utilisation du logiciel, veuillez vous référer au [manuel utilisateur](https://github.com/minh-n/Casablanca-ProjetGL/blob/master/Documents/Documents%20du%20rendu/%5BET5-GL%5D%20Manuel%20utilisateur.pdf). D'autres documents complémentaires peuvent être trouvés dans le dossier Document de ce répertoire.

Au commencement, le logiciel ne contient ni de collaborateurs, ni de service, sauf pour les services Direction, RH et Comptabilité.

Un compte administrateur est fourni à l’entreprise : des services et collaborateurs pourront être créés. Une fois leur création complétée, les utilisateurs pourront se servir du logiciel et créer leurs premières demandes de congé ou notes de frais.

### Equipe

L'équipe de développement du projet est constituée de :
  - [@JeffreGoncalves](https://github.com/JeffreGoncalves) (Jeffrey G.) : demandes d'avance, tests
  - [@Drakyll](https://github.com/Drakyll) (Morgan F.) : back-end, notes de frais, missions, traitement 
  - [@Veados](https://github.com/Veados) (Adrien L.) : back-end, notifications
  - [@creatingame](https://github.com/creatingame) (Yao S.) : fonctions administrateur
  - [@minh-n](https://github.com/minh-n) (Minh N.) : notes de frais, demandes de congé, coordination du groupe
  
  

