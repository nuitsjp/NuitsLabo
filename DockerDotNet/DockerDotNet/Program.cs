using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace DockerDotNet
{
    class Program
    {
        const string ImageName = "nginx";
        const string Tag = "latest";

        static async Task Main(string[] args)
        {
            Console.WriteLine("create docker client.");
            var dockerApiUri = GetDockerApiUri();
            var dockerClient = 
                new DockerClientConfiguration(dockerApiUri).CreateClient();

            Console.WriteLine("pull image...");
            await PullImage(dockerClient);

            Console.WriteLine("start container.");
            var containerId = await StartContainerAsync(dockerClient);

            Console.WriteLine("Open http://localhost:8080 in your browser. And Press Enter.");
            Console.ReadLine();

            Console.WriteLine("stop container.");
            await StopContainerAsync(dockerClient, containerId);
        }

        private static async Task PullImage(DockerClient dockerClient)
        {
            await dockerClient.Images
                .CreateImageAsync(new ImagesCreateParameters
                    {
                        FromImage = ImageName,
                        Tag = Tag
                    },
                    new AuthConfig(),
                    new Progress<JSONMessage>());
        }

        private static async Task<string> StartContainerAsync(DockerClient dockerClient)
        {
            var response = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = ImageName,
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    {
                        "80", default(EmptyStruct)
                    }
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {"80", new List<PortBinding> {new PortBinding {HostPort = "8080" } }}
                    },
                    PublishAllPorts = true
                }
            });
            await dockerClient.Containers.StartContainerAsync(response.ID, null);
            return response.ID;
        }

        private static async Task StopContainerAsync(DockerClient dockerClient, string containerId)
        {
            await dockerClient.Containers.KillContainerAsync(containerId, new ContainerKillParameters());
        }


        private static Uri GetDockerApiUri()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {
                return new Uri("npipe://./pipe/docker_engine");
            }

            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isLinux)
            {
                return new Uri("unix:/var/run/docker.sock");
            }

            throw new Exception("Was unable to determine what OS this is running on, does not appear to be Windows or Linux!?");
        }
    }
}
