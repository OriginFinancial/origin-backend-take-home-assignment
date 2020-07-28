# Origin Backend Take-Home Assignment

This challenge was developed using Typescript and Nodejs, based on best pratics of coding [SOLID](https://medium.com/joaorobertopb/o-que-%C3%A9-solid-o-guia-completo-para-voc%C3%AA-entender-os-5-princ%C3%ADpios-da-poo-2b937b3fc530) and [Clean Code](https://medium.com/joaorobertopb/2-clean-code-boas-pr%C3%A1ticas-para-escrever-c%C3%B3digos-impec%C3%A1veis-361997b3c8b5). The project was designed using [DDD](https://en.wikipedia.org/wiki/Domain-driven_design) (Domain Driven Design).

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
