# Halving Carousel

The purpose of this exercise is to train you to use extend classes with `extends` keyword.

Estimated workload of this exercise is _30 min_.

Note, that if you have not done the "Decrementing Carousel" exercise,
you have to implement `DecrementingCarousel` and `CarouselRun` classes.
Details are specified in [DC.md](DC.md).

### Description

In this exercise you need to extend [`DecrementingCarousel`](src/main/java/com/epam/rd/autotasks/DecrementingCarousel.java).
You need to implement [`HalvingCarousel`](src/main/java/com/epam/rd/autotasks/HalvingCarousel.java).
This subclass must halve elements instead of decrementing it by one.
Note that you need to apply regular integer division, discarding the remainder.
For example, `5 / 2 = 2`.

### Examples

Empty case:
```java
CarouselRun run = new HalvingCarousel(7).run();
System.out.println(run.isFinished()); //true
System.out.println(run.next()); //-1
```

Regular case:
```java
DecrementingCarousel carousel = new HalvingCarousel(7);

carousel.addElement(20);
carousel.addElement(30);
carousel.addElement(10);

CarouselRun run = carousel.run();

System.out.println(run.isFinished()); //false

System.out.println(run.next()); //20
System.out.println(run.next()); //30
System.out.println(run.next()); //10

System.out.println(run.next()); //10
System.out.println(run.next()); //15
System.out.println(run.next()); //5

System.out.println(run.next()); //5
System.out.println(run.next()); //7
System.out.println(run.next()); //2

System.out.println(run.next()); //2
System.out.println(run.next()); //3
System.out.println(run.next()); //1

System.out.println(run.next()); //1
System.out.println(run.next()); //1

System.out.println(run.isFinished()); //true
System.out.println(run.next()); //-1
```