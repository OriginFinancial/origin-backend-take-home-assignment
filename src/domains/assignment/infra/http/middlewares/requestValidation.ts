import { celebrate, Segments, Joi } from 'celebrate';

const requestValidation = celebrate({
  [Segments.BODY]: {
    age: Joi.number().min(0).required(),
    dependents: Joi.number().min(0).required(),
    house: Joi.object().keys({
      ownership_status: Joi.string().valid('owned', 'mortgaged'),
    }),
    income: Joi.number().min(0).required(),
    marital_status: Joi.string().required().valid('single', 'married'),
    risk_questions: Joi.array()
      .length(3)
      .required()
      .items(Joi.number().valid(0, 1)),
    vehicle: Joi.object().keys({
      year: Joi.number().max(new Date().getFullYear()),
    }),
  },
});

export default requestValidation;
