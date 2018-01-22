using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IsMatch.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace IsMatch
{
    /// <summary>
    /// 应用程序的入口
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 配置用于应用程序内的服务(可选) 需要再 Configure 前调用
        /// </summary>
        /// <param name="services">当前容器中各服务的配置集合</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加配置参数映射
            services.Configure<Param>(Configuration);

            services.AddMvc();
        }

        /// <summary>
        /// 指定 Asp.Net 程序如何响应每一个Http请求
        /// </summary>
        /// <param name="app">必须接收的IApplicationBuilder参数(构建应用程序的请求管道)</param>
        /// <param name="env">额外服务</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();// 中间件的扩展方法 添加到IApplicationBuilder(请求管道)上
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();// for the wwwroot folder

            // 显示其他文件目录的内容(懒得建文件夹了,所以还是拿了wwwroot文件夹测试)
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\images")),
                RequestPath = new PathString("/ismatchImg")                
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
