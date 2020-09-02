package com.ogs.origin.service;

import com.ogs.origin.exception.OriginException;
import com.ogs.origin.model.*;
import org.springframework.stereotype.Service;

import java.time.LocalDate;

@Service
public class OriginServiceImpl implements OriginService {

    @Override
    public RiskProfile calculateRisk(PersonalInformation personalInformation) throws OriginException {

        validateFields(personalInformation);

        return calculateScore(personalInformation);
    }

    private void validateFields(PersonalInformation personalInformation) throws OriginException {
        if (personalInformation.getAge() < 0) {
            throw new OriginException("Invalid age");
        }

        if (personalInformation.getDependents() < 0) {
            throw new OriginException("Invalid dependents");
        }

        if (personalInformation.getIncome() < 0) {
            throw new OriginException("Invalid income");
        }
    }

    private RiskProfile calculateScore(PersonalInformation personalInformation) {
        Score disability = null;
        Score auto = null;
        Score home = null;
        Score life = null;
        int autoScore = 0;
        int disabilityScore = 0;
        int homeScore = 0;
        int lifeScore = 0;

        if (
                personalInformation.getIncome() < 0 |
                personalInformation.getVehicle() == null |
                personalInformation.getHouse() == null
        ) {
            disability = Score.INELIGIBLE;
            auto = Score.INELIGIBLE;
            home = Score.INELIGIBLE;
        }

        if (personalInformation.getAge() > 60) {
            disability = Score.INELIGIBLE;
            life = Score.INELIGIBLE;
        }

        if (allFieldsAreIneligible(disability, life, home, auto)){
            return new RiskProfile(life, disability, home, auto);
        }

        autoScore = disabilityScore = homeScore = lifeScore = calculateBaseScore(personalInformation.getRisk_questions());

        if (personalInformation.getAge() < 30) {
            autoScore -= 2;
            disabilityScore -= 2;
            homeScore -= 2;
            lifeScore -= 2;
        }

        if (personalInformation.getAge() >= 30 && personalInformation.getAge() <= 40) {
            autoScore--;
            disabilityScore--;
            homeScore--;
            lifeScore--;
        }

        if (personalInformation.getIncome() > 200000) {
            autoScore--;
            disabilityScore--;
            homeScore--;
            lifeScore--;
        }

        if (personalInformation.getHouse().getOwnership_status().equals(OwnershipStatus.MORTGAGED)) {
            disabilityScore++;
            homeScore++;
        }

        if (personalInformation.getDependents() > 0) {
            disabilityScore++;
            lifeScore++;
        }

        if (personalInformation.getMarital_status() == MaritalStatus.MARRIAGE) {
            lifeScore++;
            disabilityScore--;
        }

        if (personalInformation.getVehicle().getYear() < LocalDate.now().getYear() - 5) {
            autoScore++;
        }

        return new RiskProfile(
                getScore(life, lifeScore),
                getScore(disability, disabilityScore),
                getScore(home, homeScore),
                getScore(auto, autoScore)
                );
    }

    private int calculateBaseScore(Boolean[] riskQuestions){
        int baseScore = 0;

        for (int i = 0; i < 3; i++){
            if (riskQuestions[i] == true) {
                baseScore++;
            }
        }

        return baseScore;
    }

    private Score getScore(Score score, int scoreValue){
        if (score == Score.INELIGIBLE) {
            return score;
        }else {
            if (scoreValue <= 0) {
                return Score.ECONOMIC;
            } else if (scoreValue >= 1 && scoreValue <= 2) {
                return Score.REGULAR;
            } else {
                return Score.RESPONSIBLE;
            }
        }
    }

    private boolean allFieldsAreIneligible(
            Score disability,
            Score life,
            Score home,
            Score auto
            ){
        if (
                disability == Score.INELIGIBLE &&
                life == Score.INELIGIBLE &&
                home == Score.INELIGIBLE &&
                auto == Score.INELIGIBLE
        ) {
            return true;
        }
        return false;
    }
}
