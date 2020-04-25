This directory contains source files which are common (or almost common)
to both NHibernate 4.x and NHibernate 5.x.  Instead of duplicate them
across two projects, a single source file is provided, and compiler
flags are used to deal with the (small) differences in NHibernate
versions.

Beware that there is only one copy of these files, even though they are
used by multiple projects.  This original file is LINKED to those projects
rather than duplicated.