This directory contains source files which are common (or almost common)
to both NHibernate 4.x and NHibernate 5.x.  Instead of duplicating them
across projects, a single source file is used.  Compiler flags are used
to deal with the (small) differences in NHibernate versions.

Beware that there is only one version of each file, but each is linked
to two projects.