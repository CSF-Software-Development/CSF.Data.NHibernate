This directory contains source files which are common (or almost common)
to both NHibernate 4.x and NHibernate 5.x.  Instead of duplicating them
across projects, a single source file is used.  Compiler flags are used
where applicable to deal with the differences in NHibernate versions.

Beware that this directory contains the only true copy of each source
file, but they are each linked to multiple projects.