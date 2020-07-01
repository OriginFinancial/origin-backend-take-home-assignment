package com.useorigin.insurance.api.risk.adapter.in;

import com.useorigin.insurance.api.risk.infrastructure.web.out.RiskProfileResource;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;

import java.util.Optional;

import static org.springframework.http.HttpStatus.CREATED;

@RestController
public class InsuranceController {

    @PostMapping("/insurances/risk")
    private ResponseEntity<RiskProfileResource> risk() {
        RiskProfileResource riskProfileResource = new RiskProfileResource("regular", "ineligible", "economic", "regular");
        return ResponseEntity.status(CREATED).body(riskProfileResource);
    }
}
