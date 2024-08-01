using System.CommandLine;
using System.Net;

namespace WellKnowns
{

    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var cmd = CreateCommand();

            await cmd.InvokeAsync(args);

            return 0;
        }

        private static RootCommand CreateCommand()
        {
            var cmd = new RootCommand("Scans a host for it's .well-known URIs (RFC 8615)");

            var hostArgument = new Argument<Uri>("hostUri", "Host URI to be scanned");
            cmd.AddArgument(hostArgument);

            cmd.SetHandler(ExecuteAsync, hostArgument);

            return cmd;
        }

        private static async Task ExecuteAsync(Uri host)
        {
            var httpClient = new HttpClient();

            var foundEndpoints = new List<string>();

            foreach (var wellKnown in _wellKnowns)
            {
                var response = await httpClient.GetAsync($"{host}/.well-known/{wellKnown}");

                if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    foundEndpoints.Add(wellKnown);
                }
            }

            if (!foundEndpoints.Any())
            {
                Console.WriteLine("[+] No endpoints found...");
                return;
            }
            
            Console.WriteLine("[+] Found endpoints:");
            foreach (var item in foundEndpoints)
            {
                Console.WriteLine($"\t{item}");
            }
        }

        // https://www.iana.org/assignments/well-known-uris/well-known-uris.xhtml
        private static readonly List<string> _wellKnowns = new List<string>
        {
            "acme-challenge",
            "amphtml",
            "appspecific",
            "ashrae",
            "assetlinks.json",
            "brski",
            "caldav",
            "carddav",
            "change-password",
            "cmp",
            "coap",
            "core",
            "csaf",
            "csaf-aggregator",
            "csvm",
            "did.json",
            "did-configuration.json",
            "dnt",
            "dnt-policy.txt",
            "dots",
            "ecips",
            "edhoc",
            "enterprise-network-security",
            "enterprise-transport-security",
            "est",
            "genid",
            "gpc.json",
            "gs1resolver",
            "hoba",
            "host-meta",
            "host-meta.json",
            "hosting-provider",
            "http-opportunistic",
            "idp-proxy",
            "jmap",
            "keybase.txt",
            "knx",
            "looking-glass",
            "masque",
            "matrix",
            "mercure",
            "mta-sts.txt",
            "mud",
            "nfv-oauth-server-configuration",
            "ni",
            "nostr.json",
            "oauth-authorization-server",
            "ohttp-gateway",
            "open-resource-discovery",
            "openid-configuration",
            "openorg",
            "oslc",
            "pki-validation",
            "posh",
            "private-token-issuer-directory",
            "probing.txt",
            "pvd",
            "rd",
            "related-website-set.json",
            "reload-config",
            "repute-template",
            "resourcesync",
            "sbom",
            "security.txt",
            "ssf-configuration",
            "sshfp",
            "stun-key",
            "terraform.json",
            "thread",
            "time",
            "timezone",
            "tdmrep.json",
            "tor-relay",
            "tpcd",
            "traffic-advice",
            "trust.txt",
            "uma2-configuration",
            "void",
            "webfinger",
            "webweaver.json",
            "wot"
        };

    }
}
