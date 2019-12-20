using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace ZmToYnab.Models.YNAB
{
    public struct YnabCategoriesData
    {
        [JsonProperty("data")]
        public CategoriesData Data { get; set; }
    }

    public struct CategoriesData
    {
        [JsonProperty("category_groups")]
        public List<CategoryGroup> CategoryGroups { get; set; }
    }

    public struct CategoryGroup
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("categories")]
        public List<YnabCategory> Categories { get; set; }
    }

    public struct YnabCategory
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("category_group_id")]
        public Guid CategoryGroupId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("original_category_group_id")]
        public object OriginalCategoryGroupId { get; set; }

        [JsonProperty("note")]
        public object Note { get; set; }

        [JsonProperty("budgeted")]
        public long Budgeted { get; set; }

        [JsonProperty("activity")]
        public long Activity { get; set; }

        [JsonProperty("balance")]
        public long Balance { get; set; }

        [JsonProperty("goal_type")]
        public object GoalType { get; set; }

        [JsonProperty("goal_creation_month")]
        public object GoalCreationMonth { get; set; }

        [JsonProperty("goal_target")]
        public long GoalTarget { get; set; }

        [JsonProperty("goal_target_month")]
        public object GoalTargetMonth { get; set; }

        [JsonProperty("goal_percentage_complete")]
        public object GoalPercentageComplete { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}
