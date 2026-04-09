namespace BirthdayWebsiteAPI.Helpers
{
    public class EntityUpdates
    {
        public void UpdateProperties<T>(T currentEntity, object newEntity)
        {
            // Get all properties of the new entity
            var properties = newEntity.GetType().GetProperties();

            foreach (var property in properties)
            {
                // Get the value of the propertiy from the new entity
                var value = property.GetValue(newEntity);
                // If the value is nor null, update the corresponding property in the User class
                if (value != null)
                {
                    // Get the corresponding property in the currentEntity (e.g., User or Guest)
                    var currentEntityProperty = typeof(T)
                        .GetProperties()
                        .FirstOrDefault(p => string.Equals(p.Name, property.Name, StringComparison.OrdinalIgnoreCase));
                    if (currentEntityProperty != null)
                        // Set the value for the corresponding property in the currentEntity object
                        currentEntityProperty.SetValue(currentEntity, value);
                }
            }
        }
    }
}
