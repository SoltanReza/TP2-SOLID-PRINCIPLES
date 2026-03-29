

SRP : Avant, ReservationService faisait tout : accès aux données, logique métier, orchestration. J'ai séparé : repository pour les données, ReservationService pour les règles métier, contrôleur pour l’orchestration. Les acteurs sont : le réceptionniste (annulation), le comptable (calcul facture), la gouvernante (planning ménage). Chacun a son module dédié, donc moins de risques de bugs croisés.

OCP : Le switch/case sur la politique d’annulation a été supprimé. Chaque règle (Flexible, Moderate, Strict, NonRefundable) est une classe qui implémente une interface. Le service reçoit la politique à utiliser par injection. On peut ajouter une nouvelle règle sans toucher au code existant.

LSP : Avant, NonRefundableReservation implémentait Cancel() mais lançait une exception. Maintenant, seules les réservations annulables ont Cancel(). Le compilateur empêche d’appeler Cancel sur une non-remboursable. Plus d’erreur au runtime, le contrat est clair.

ISP : Les interfaces trop larges ont été repérées (ex : IReservationRepository). J'ai veillé à ce que chaque service ne dépende que de ce dont il a besoin, ce qui rend le code plus souple et plus facile à faire évoluer.

DIP : Les services métiers (ex : BookingService, HousekeepingService) dépendent d’interfaces (IReservationRepository, ILogger, ICleaningNotifier) définies côté métier. Les classes techniques (FileLogger, EmailCleaningNotifier) sont injectées. On peut changer la technique sans toucher au métier, ce qui facilite la maintenance et les tests.
