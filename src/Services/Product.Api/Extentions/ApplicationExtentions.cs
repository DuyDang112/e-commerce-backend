namespace Product.Api.Extentions
{
    // IApplicationBuilder đại diện cho pipeline, xử lý HTTP request, cấu hình route
    public static class ApplicationExtentions
    {
        public static void UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            //app.UseHttpsRedirection(); // sử dụng cho môi trường product

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
