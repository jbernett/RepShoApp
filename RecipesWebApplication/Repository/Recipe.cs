//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RecipesWebApplication.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Recipe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recipe()
        {
            this.RecipeIngredients = new HashSet<RecipeIngredient>();
            this.RecipeSteps = new HashSet<RecipeStep>();
        }
    
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
        public System.TimeSpan PrepTime { get; set; }
        public System.TimeSpan CookTime { get; set; }
        public System.TimeSpan TotalTime { get; set; }
        public int RecipeCategoryID { get; set; }
        public string RecipeImage { get; set; }
        public int IsHidden { get; set; }
    
        public virtual RecipeCategory RecipeCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipeStep> RecipeSteps { get; set; }
    }
}
