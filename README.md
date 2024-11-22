# WPC-2024 - DDD & CQRS/ES with .NET

## Domain-Driven Design
Are DDD patterns still valid in 2024?  
Eric Evans published the Blue Book in August 2003, but the core concepts of Domain-Driven Design remain highly relevant.  
They continue to provide valuable guidance in creating systems that effectively solve business problems.

## DDD is Just for Complex Solution
This is true to some extent.  
However, you can still begin your project by applying strategic patterns that lay the foundation for a maintainable solution.  
At the start of any project, your understanding of the domain is limited, making it difficult to make informed architectural decisions.  
That’s why it’s essential to design a solution that can evolve over time—even in a microservices architecture,  
and DDD offers the right patterns to support this adaptability.

## CQRS/ES
No, this is false!  
You can apply CQRS without using Event Sourcing, or even choose not to implement the CQRS pattern at all.  
However, incorporating Domain and Integration Events can be beneficial if you anticipate that your solution will evolve into a distributed system.

