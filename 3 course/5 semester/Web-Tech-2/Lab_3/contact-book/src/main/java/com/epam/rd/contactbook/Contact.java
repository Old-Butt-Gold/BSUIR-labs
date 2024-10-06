package com.epam.rd.contactbook;

public class Contact {

    private class NameContactInfo implements ContactInfo {

        @Override
        public String getTitle() {
            return "Name";
        }

        @Override
        public String getValue() {
            return contactName;
        }
    }

    public static class Email implements ContactInfo {
        String email;

        public Email(String email) {
            this.email = email;
        }

        @Override
        public String getTitle() {
            return "Email";
        }

        @Override
        public String getValue() {
            return email;
        }
    }

    public static class Social implements ContactInfo {

        String title;
        String name;

        public Social(String title, String name) {
            this.title = title;
            this.name = name;
        }

        @Override
        public String getTitle() {
            return title;
        }

        @Override
        public String getValue() {
            return name;
        }
    }

    String contactName;

    ContactInfo nameInfo;
    ContactInfo phoneNumberInfo;

    ContactInfo[] emails = new ContactInfo[3];

    ContactInfo[] socials = new ContactInfo[5];

    int currentEmailIndex = 0;
    int currentSocialIndex = 0;

    public Contact(String contactName) {
        this.contactName = contactName;
        this.nameInfo = new NameContactInfo();
    }

    public void rename(String newName) {
        if (newName == null || newName.isEmpty()) return;
        contactName = newName;
    }

    public Email addEmail(String localPart, String domain) {
        if (currentEmailIndex == emails.length) {
            return null;
        }

        Email email = new Email(localPart + "@" + domain);
        emails[currentEmailIndex++] = email;
        return email;
    }

    public Email addEpamEmail(String firstname, String lastname) {
        if (currentEmailIndex == emails.length) {
            return null;
        }
        Email epamEmail = new Email(firstname + "_" + lastname + "@epam.com") {
            @Override
            public String getTitle() {
                return "Epam Email";
            }
        };
        emails[currentEmailIndex++] = epamEmail;
        return epamEmail;
    }

    public ContactInfo addPhoneNumber(int code, String number) {
        if (phoneNumberInfo != null) {
            return null;
        }
        phoneNumberInfo = new ContactInfo() {
            @Override
            public String getTitle() {
                return "Tel";
            }

            @Override
            public String getValue() {
                return "+" + code + " " + number;
            }
        };
        return phoneNumberInfo;
    }

    public Social addTwitter(String twitterId) {
        if (currentSocialIndex == socials.length) {
            return null;
        }

        Social twitter = new Social("Twitter", twitterId);
        socials[currentSocialIndex++] = twitter;
        return twitter;
    }

    public Social addInstagram(String instagramId) {
        if (currentSocialIndex == socials.length) {
            return null;
        }

        Social instagram = new Social("Instagram", instagramId);
        socials[currentSocialIndex++] = instagram;
        return instagram;
    }

    public Social addSocialMedia(String title, String id) {
        if (currentSocialIndex == socials.length) {
            return null;
        }

        Social social = new Social(title, id);
        socials[currentSocialIndex++] = social;
        return social;
    }

    public ContactInfo[] getInfo() {
        int count = 1;
        if (phoneNumberInfo != null) {
            count++;
        }
        count += currentSocialIndex + currentEmailIndex;

        ContactInfo[] contactInfos = new ContactInfo[count];
        int currentIndex = 0;
        contactInfos[currentIndex++] = nameInfo;
        if (phoneNumberInfo != null) {
            contactInfos[currentIndex++] = phoneNumberInfo;
        }

        for (int i = 0; i < currentEmailIndex; i++) {
            contactInfos[currentIndex++] = emails[i];
        }

        for (int i = 0; i < currentSocialIndex; i++) {
            contactInfos[currentIndex++] = socials[i];
        }

        return contactInfos;
    }

}
