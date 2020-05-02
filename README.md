*Please be aware that much of the former functionality in this repository has moved to the **[CSF.ORM]** repository.  If you can't find what you were looking for, please look there instead.*

[CSF.ORM]: https://github.com/csf-dev/CSF.ORM

## CSF.NHibernate
This repository contains four small pieces of [NHibernate]-related functionality.  Each is a little too small to have its own repository, although they are packaged independently.  Most of these packages have *two versions* available.  One for NHibernate version 4.x and one for NHibernate version 5.x.

*   **CSF.NHibernate4.Guids** & **CSF.NHibernate5.Guids**

    These packages contain implementations of `NHibernate.UserTypes.IUserType` for
    storing instances of `System.Guid` in a binary-data database column.  Specifically,
    they are stored in [RFC-4122] format.

*   **CSF,NHibernate4.PrimesAndFractions** & **CSF,NHibernate5.PrimesAndFractions**

    These packages contain implementations of `NHibernate.UserTypes.IUserType` for
    storing instances of `CSF.Fraction` (see [CSF.PrimesAndFractions]) in a database.

*   **CSF.NHibernate4.MonoSafeSQLite20Driver** & **CSF.NHibernate5.MonoSafeSQLite20Driver**

    These packages contain a type named `CSF.NHibernate.MonoSafeSQLite20Driver` which
    works around an incompatibility between the Mono runtime on Linux and .NET Framework
    on Windows where it comes to the [SQLite] ADO database driver. Mono ships with a bundled
    version of this driver, compatible with Mono.  The driver available via NuGet is not
    compatible with Mono and will cause errors.  The Mono-safe NHibernate driver reconciles
    this by detecting and using the appropriate driver for the currently-executing runtime.

*   **CSF.NHibernate.Unproxy**

    This is a small convenience service, `CSF.NHibernate.ObjectUnproxyingService`, which
    uses an NHibernate ISession to 'unproxy' an object which could be an NHibernate proxy.

[NHibernate]: https://github.com/nhibernate/nhibernate-core
[RFC-4122]: https://en.wikipedia.org/wiki/Universally_unique_identifier
[CSF.PrimesAndFractions]: https://github.com/csf-dev/PrimesAndFractions
[SQLite]: https://www.sqlite.org/index.html

## Open source license
All source files within this project are released as open source software,
under the terms of [the MIT license].

[the MIT license]: http://opensource.org/licenses/MIT
