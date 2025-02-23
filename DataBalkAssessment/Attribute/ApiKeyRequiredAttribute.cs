using System;

namespace DataBalkAssessment.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyRequiredAttribute : Attribute
    {
        // You can add additional properties or logic here if needed
    }
}
