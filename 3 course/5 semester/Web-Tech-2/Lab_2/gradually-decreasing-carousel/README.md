# Gradually Decreasing Carousel

The purpose of this exercise is to train you to use extend classes with `extends` keyword.

Estimated workload of this exercise is _30 min_.

Note, that if you have not done the "Decrementing Carousel" exercise,
you have to implement `DecrementingCarousel` and `CarouselRun` classes.
Details are specified in [DC.md](DC.md).

### Description

In this exercise you need to extend [`DecrementingCarousel`](src/main/java/com/epam/rd/autotasks/DecrementingCarousel.java).
You need to implement [`GraduallyDecreasingCarousel`](src/main/java/com/epam/rd/autotasks/GraduallyDecreasingCarousel.java).
This subclass must decrement elements by gradually increasing decrement.
When you need to decrement an element for the first time, decrease it by `1`.
Next time you need to decrement the same element, decrease it by `2`.
Next time decrease by `3`, then by `4` and so on. 
Remember that you must not make process non-positive elements.


### Examples

Empty case:
```java
CarouselRun run = new GraduallyDecreasingCarousel(7).run();
System.out.println(run.isFinished()); //true
System.out.println(run.next()); //-1
```

Regular case:
```java
DecrementingCarousel carousel = new GraduallyDecreasingCarousel(7);

carousel.addElement(20);
carousel.addElement(30);
carousel.addElement(10);

CarouselRun run = carousel.run();

System.out.println(run.isFinished()); //false

System.out.println(run.next()); //20
System.out.println(run.next()); //30
System.out.println(run.next()); //10

System.out.println(run.next()); //19
System.out.println(run.next()); //29
System.out.println(run.next()); //9

System.out.println(run.next()); //17
System.out.println(run.next()); //27
System.out.println(run.next()); //7

System.out.println(run.next()); //14
System.out.println(run.next()); //24
System.out.println(run.next()); //4

System.out.println(run.next()); //10
System.out.println(run.next()); //20

System.out.println(run.next()); //5
System.out.println(run.next()); //15

System.out.println(run.next()); //9

System.out.println(run.next()); //2

System.out.println(run.isFinished()); //true
System.out.println(run.next()); //-1
```