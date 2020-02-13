## Moved functionality
Please note that a lot of the functionality which has been in this library is moving to **[CSF.ORM](https://github.com/csf-dev/CSF.ORM)**.  Further development will be found there.

*It was found that a number of repositories were so closely linked that they should be converted to a single repository, even though they are multiple separate NuGet packages.*

---

# CSF.Data.NHibernate
This library contains types which are useful for interacting with [the NHibernate ORM].

[the NHibernate ORM]: http://nhibernate.info/

## IQuery implementation
The main contents of this library is an implementation of `CSF.Data.IQuery`, which wraps an NHibernate `ISession`; this is the **NHibernateQuery** type.
This allows an application to construct Linq queries from the ISession with two related benefits:

* Firstly, it does not require a direct reference to the ISession, so alternate query implementations may be substituted.
* Via a test-fake, this is not tied to NHibernate's extension methods, thus queries may be more easily mocked.

### NHibernate IQueryable<T> method wrappers
Additionally to the above are a number of supporting types which permit usage of NHibernate's Linq API.
This includes extension methods which wrap calls to `Fetch`, `FetchMany`, `ThenFetch` and `ThenFetchMany`, but which do not  raise exceptions if the query is not powered by a real NHibernate ISession.
Likewise there are wrappers for `ToFuture` and `ToFutureValue` which do not require direct usage of NHibernate's own extension methods.

All of the wrapped functionality above becomes a no-operation if the query is not powered by a real NHibernate ISession.
If it is a real ISession, then the appropriate functionality is called using NHibernate.

### AnyCount
Finally, related to `IQuery`, is an extension method named `.AnyCount()`.
This will behave just like Linq `.Any()` except that it forces counting of all of the instances and then returning truth if there is more than zero.
For some database tables (such as legacy databases), where tables are very wide (many columns), NHibernate's default strategy for handling Linq 'Any' might not be suitable.
By default, NHibernate performs a select-all-columns, limited to only the first row, and then returns true if it received any data.
With extremely wide (but not very tall) tables though, and particularly where network bandwidth is limited, it may be cheaper to request the database perform a count and then act based on this information.

Please use AnyCount judiciously, it is not a silver bullet and your mileage may vary.
The recommendation is, use the standard Linq `.Any()` as your go-to, and only try with `.AnyCount()` if you are noticing a serious performance hit.

## Other types
There are some other miscellaneous small types in this library:

* **BinaryGuidType** - an `IUserType` for storing `System.Guid` instances as binary data.
* **FractionType** - an `IUserType` for storing `CSF.Fraction` instances.
* An extension method for `IDbIntegrationConfigurationProperties` - `SelectSQLiteDriver()` - which chooses either the Windows or Mono version of the driver.

## Open source license
All source files within this project are released as open source software,
under the terms of [the MIT license].

[the MIT license]: http://opensource.org/licenses/MIT

This software is distributed in the hope that it will be useful, but please
remember that:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
