# Dissertation Structure Plan

## Introduction

- AR and XR technology is becoming more popular in the general market
- Has a variety of potential use cases
- Mostly used for gaming and industrial purposes
- Many potential benefits for the average person
- Hololens is much like the newly released Apple Vision Pro but with slightly older technology
- Benefits of pass through and being able to see digital content in you POV
- People struggle with assembling flatpack furniture
- "Transforming Issues in Housing Design" reference
- Could AR enhancement be the solution?

## Background

- Advancements of AR and XR technology
- Wearable headsets
- The effectiveness of these in helping people with tasks
- Is it a proven method to help people?

## Analysis/Requirements

- What the thing needed to do/have and how you found it out

## Design



## Implementation

### Choosing Platforms
- There are a range of frameworks for creating AR projects
### Unity
- Unity was the best option as I have the most experience with it and have previously created an AR project in it
- Unity AR packages
- Compatability with a object tracking frameworks
### Tracking Method
- Multiple options for tracking objects in the Hololens
- Had to be compatible with Unity and Hololens
- Needed to be quite reliable especially with people holding the parts
#### Unity Image Tracking
- Testing showed that objects would stick to images quite reliably
- Loses tracking when object is rotated away from imahge
- Could have an image on each surface to solve this
- Not very viable in the real market as people wont have images all over their furniture
#### Azure Object Anchors
- Framework for tracking 3D models in real life
- Required credits to use
- Needed specifically formatted 3D models that had to be converted and saved on the Hololens to use
- 3D models were limited to a particular size and certain amount of them at a time
- Testing showed it to be extremely difficult to use and set up
- Being shut down in May
#### VisionLib
- Another framework for tracking 3D models in real life
- Very flexible to use
- Integrates very well with Unity
- Any model that can be rendered in Unity can be tracked
- Can track multiple models combined into one
- Works well with Hololens but can also be used on almost any camera as long as it has been calibrated
- Testing showed it to be fairly reliable in well lit areas
- Tended to lose tracking if hands or arms covered sections of the view
- Requires an init pose
- Locked behind a restrictive choice of licences
- Must directly communicate with organisation to even get a price 
### Licensing Troubles
- VisionLib clear best choice
- Reached out to Visiometry to enquire about licence prices
- They offered a educational licence for a suitable period of time
- They required cetrain IDs from me to grant licence
- Became unresponsive at this point
- Delayed work for weeks as they continued to not respond to emails
- Accepted that I would have to go ahead with Azure Object Anchors
- This would likely result in a lower quality final piece of software
- Just as I began to implement it VisionLib responded saying they tried to send licence but email service had blocked the file
- This meant that I was able to use VisionLib but it had set me behind a good few weeks
### Setting up project
- Unity with all the right packages
- MRTK
- Right version of Visual Studio and Windows 10 SDK
### Scene Setup
- Components required for scene
- Setting camera height
### Custom Framework for Instructions
- Discuss object design (Part, Component, TrackingPart, AnimationPart, Furniture, Instructions)
### Input
- Voice and keyboard commands
### Custom 3D models
- Lack of online availability
- Process of creating 3D model
- Structure of .blend file
- Process of importing to Unity
### Setup of bedsideTable Prefab

## Experiment Design

### Experiment Options
- Ideas that were proposed
- Difficulty in choosing best method
### Pilot Test
- Decided to conduct a pilot test
- Details of pilot test
### Final Design
- Decision based on outcome of pilot test
- Details of how experiment was carried out
- Split participants into three groups
- One group built using paper instructions, one using animation version, one using tracking version
- Used think aloud with each participant
- Participants said out loud each time they felt a moments of confusion and explain why
- Thoughts were categorised into groups (e.g. which piece, part orientation, positioning, errors)
- Each time an error is made is tracked too
- Number and types of errors compared accross the groups

## Evaluation

## Conclusion
