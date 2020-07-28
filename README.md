# Origin Backend Take-Home Assignment

This challenge was developed using Typescript and Nodejs, based on best pratics of coding [SOLID](https://en.wikipedia.org/wiki/SOLID), [DRY](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself)(Don't Repeat Yourself) and [Clean Code](https://medium.com/joaorobertopb/2-clean-code-boas-pr%C3%A1ticas-para-escrever-c%C3%B3digos-impec%C3%A1veis-361997b3c8b5). The project was designed using [DDD](https://en.wikipedia.org/wiki/Domain-driven_design) (Domain Driven Design).

### Instalation:
This project depends on [docker-compose](https://docs.docker.com/compose/install/) and [yarn](https://classic.yarnpkg.com/en/docs/install/#debian-stable) installed on local machine.
```sh
$ cd origin-backend-take-home-assignment
$ yarn
$ sudo docker-compose up --build -d
```
After instalation, the API is served on: http://localhost:3333

### Tests
This project contains unit tests and integration tests, to run the tests use:
```sh
sudo docker exec -i origin_nodejs /bin/sh -c "yarn test"
```
OR
```sh
yarn test
```
