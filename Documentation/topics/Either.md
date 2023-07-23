# Either

``Either<TLeft, TRight>`` represents *either* type ``TLeft`` or ``TRight``, but never both or none.

## Signature

```C#
public readonly struct Either<TLeft, TRight>
```

## Methods

Describe what each option is used for:


<tabs>
    <tab title="Method Signature">
        <code-block lang="c#">
            public Either<TNewLeft, TNewRight> Map<\TNewLeft, TNewRight>(Func<TLeft, TNewLeft> left, Func<TRight, TNewRight> right)
        </code-block>
    </tab>
    <tab title="Example">
        <code-block lang="c#">
            <![CDATA[<img src="new_topic_options.png" alt="Alt text" width="450px"/>]]></code-block>
    </tab>
</tabs>

<deflist type="medium">
            <def title="-o, --open">
                Opens a file.
            </def>
            <def title="-c, --close">
                Closes a file.
            </def>
            <def title="-v --version">
                Displays version information.
            </def>
            <def title="-h, --help">
                Displays help.
            </def>
</deflist>

<seealso>
    <!--Provide links to related how-to guides, overviews, and tutorials.-->
</seealso>