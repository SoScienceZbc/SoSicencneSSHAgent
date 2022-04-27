using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Proto;
using SoSicencneSSHAgent.MicroServices;

namespace SoSicencneSSHAgent
{
    class VideoService : RemoteMediaService.RemoteMediaServiceBase
    {
        private static VideoMicroService videoHandler = new VideoMicroService();

        public override Task<VideoReply> SendVideo(VideoRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Host:{context.Host} called Method:{context.Method}");
            return videoHandler.SendVideo(request);
        }
    }
}
