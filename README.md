# LightFlickerController

The `LightFlickerController` script is a Unity component that controls the flickering of a light to simulate effects like a faulty light bulb. It can be used to add atmosphere and realism to scenes in Unity games.

## Features

- **Customizable Flicker Duration:** Set the minimum and maximum duration for the light to stay on or off during a flicker.
- **Customizable Off Duration:** Set the minimum and maximum duration for the light to stay off between flickers.
- **Repeat Count:** Specify the number of times the light flickers before stopping.
- **Stay On After Complete:** Choose whether the light remains on after completing the flickering sequence.
- **Start On Awake:** Decide whether flickering starts automatically when the object awakens.
- **Events:** Trigger UnityEvents at key moments, such as when the flickering starts, stops, or when the light turns on or off.

## Usage

1. Attach the `LightFlickerController` script to a GameObject with a Light component.
2. Adjust the script parameters in the Inspector to configure the flickering behavior.
3. Use the `StartFlickering`, `StopFlickering`, and `ResetFlickering` methods to control the flickering effect programmatically.

## Example

```csharp
public class LightFlickerExample : MonoBehaviour
{
    private LightFlickerController lightFlickerController;

    void Start()
    {
        lightFlickerController = GetComponent<LightFlickerController>();
        lightFlickerController.StartFlickering();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lightFlickerController.ResetFlickering();
        }
    }
}
```
