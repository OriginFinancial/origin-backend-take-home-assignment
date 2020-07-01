package com.useorigin.insurance.api.risk.application.in.command;

import javax.validation.ConstraintValidator;
import javax.validation.ConstraintValidatorContext;

public class RiskQuestionsValidator implements ConstraintValidator<RiskQuestionValid, Integer[]> {

    @Override
    public boolean isValid(Integer[] value, ConstraintValidatorContext context) {
        if (value == null)
            return false;

        for (Integer v : value)
            if (v < 0 || v > 1)
                return false;

        return true;
    }
}
