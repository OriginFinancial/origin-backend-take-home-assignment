package com.ogs.origin.service;

import com.ogs.origin.exception.OriginException;
import com.ogs.origin.model.PersonalInformation;
import com.ogs.origin.model.RiskProfile;

public interface OriginService {
    RiskProfile calculateRisk(PersonalInformation personalInformation) throws OriginException;
}
